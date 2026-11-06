# Pruebas de integracion de la API (63 verificaciones).
# Requisitos: Python 3 y la API corriendo contra una base NUEVA (con los datos semilla), por ejemplo:
#   set ConnectionStrings__Local=Server=.\SQLEXPRESS;Database=kicket_pruebas;Trusted_Connection=True;TrustServerCertificate=True;
#   dotnet run --project WebApi --urls http://localhost:5299
# Uso: python docs/pruebas/pruebas_api.py 5299 <carpeta donde guardar reporte.pdf y reporte.xlsx>
# Ojo: crea una compra y un usuario de prueba, no usar contra la base real.
import json, sys, urllib.request, urllib.error

BASE = f"http://localhost:{sys.argv[1]}"
SALIDA = sys.argv[2]
ok = fallas = 0


def req(metodo, ruta, token=None, cuerpo=None, crudo=False):
    data = json.dumps(cuerpo).encode() if cuerpo is not None else None
    r = urllib.request.Request(BASE + ruta, data=data, method=metodo)
    r.add_header("Content-Type", "application/json")
    if token:
        r.add_header("Authorization", "Bearer " + token)
    try:
        with urllib.request.urlopen(r) as resp:
            cuerpo = resp.read()
            return resp.status, (cuerpo if crudo else (json.loads(cuerpo) if cuerpo else None)), resp.headers
    except urllib.error.HTTPError as e:
        texto = e.read().decode(errors="replace")
        try:
            return e.code, json.loads(texto), e.headers
        except Exception:
            return e.code, texto, e.headers


def check(nombre, condicion, detalle=""):
    global ok, fallas
    if condicion:
        ok += 1
        print(f"  OK   {nombre}")
    else:
        fallas += 1
        print(f"  FAIL {nombre} {detalle}")


def login(email, clave):
    st, b, _ = req("POST", "/auth/login", cuerpo={"email": email, "pass": clave})
    return b["token"] if st == 200 else None, b


admin, _ = login("EmailRaro@gmail.com", "Has")
lucia, lresp = login("lucia.fernandez@mail.com", "Has")
martin, _ = login("martin.gomez@mail.com", "Has")
check("login admin/lucia/martin", admin and lucia and martin)
check("login devuelve DNI propio", lresp.get("usuario", {}).get("dni") == "38456789", lresp)

print("== Paginacion")
st, b, _ = req("GET", "/clubes/paginado?tamanoPagina=2")
check("clubes pag 1 de 2", st == 200 and b["totalRegistros"] == 4 and len(b["items"]) == 2 and b["totalPaginas"] == 2, b)
primeros = [c["clubId"] for c in b["items"]]
st, b2, _ = req("GET", "/clubes/paginado?tamanoPagina=2&pagina=2")
check("pag 2 sin repetir registros", st == 200 and not set(primeros) & {c["clubId"] for c in b2["items"]}, b2)
st, b, _ = req("GET", "/clubes/paginado?tamanoPagina=2&pagina=99")
check("pagina fuera de rango -> ultima valida", st == 200 and b["pagina"] == 2, b)
st, b, _ = req("GET", "/clubes/paginado?ordenarPor=fundacion&descendente=true")
check("orden por fundacion desc", st == 200 and b["items"][0]["anioFundacion"] == 1905, b)
st, b, _ = req("GET", "/clubes/paginado?ordenarPor=DROP%20TABLE")
check("orden fuera de lista blanca -> 400", st == 400, (st, b))
st, b, _ = req("GET", "/clubes/paginado?tamanoPagina=500")
check("tamano > 100 -> 400", st == 400, st)
st, b, _ = req("GET", "/clubes/paginado?busqueda=%27%3B%20DROP%20TABLE%20Clubs%3B--")
check("busqueda con SQL malicioso no rompe (0 resultados)", st == 200 and b["totalRegistros"] == 0, b)

print("== Filtros")
st, b, _ = req("GET", "/clubes/paginado?ciudad=Rosario")
check("clubes por ciudad", st == 200 and b["totalRegistros"] == 2, b)
st, b, _ = req("GET", "/estadios/paginado?busqueda=gigante")
check("estadio por nombre + capacidad calculada", st == 200 and b["totalRegistros"] == 1 and b["items"][0]["capacidadTotal"] == 14000, b)
st, b, _ = req("GET", "/sectores/paginado?tipo=Platea")
check("sectores tipo Platea", st == 200 and b["totalRegistros"] == 2 and all("Platea" in s["nombre"] for s in b["items"]), b)
st, b, _ = req("GET", "/sectores/paginado?tipo=Popular&estadioId=-2")
check("filtros combinados sector", st == 200 and b["totalRegistros"] == 1 and b["items"][0]["nombre"] == "Popular Norte", b)
st, b, _ = req("GET", "/eventos/paginado?estado=proximos")
check("eventos proximos (excluye pasado y cancelado)", st == 200 and b["totalRegistros"] == 2, b)
st, b, _ = req("GET", "/eventos/paginado?estado=cancelados")
check("eventos cancelados", st == 200 and b["totalRegistros"] == 1, b)
st, b, _ = req("GET", "/eventos/paginado?busqueda=River&estado=todos")
check("eventos por nombre de club", st == 200 and b["totalRegistros"] == 2 and b["items"][0]["nombreEquipoVisitante"] == "River Plate", b)
st, b, _ = req("GET", "/eventos/paginado?desde=2027-01-01&hasta=2027-04-30")
check("eventos rango de fechas", st == 200 and b["totalRegistros"] == 1, b)
st, b, _ = req("GET", "/eventos/paginado?desde=2027-05-01&hasta=2027-01-01")
check("rango invertido -> 400", st == 400, st)
st, b, _ = req("GET", "/usuarios/paginado?busqueda=38.456", admin)
check("usuarios por DNI con puntos", st == 200 and b["totalRegistros"] == 1 and b["items"][0]["email"].startswith("lucia"), b)
st, b, _ = req("GET", "/usuarios/paginado?rol=Cliente", admin)
check("usuarios por rol", st == 200 and b["totalRegistros"] == 2, b)
check("usuarios sin password en JSON", "password" not in json.dumps(b).lower() and "pass" not in json.dumps(b).lower())
st, b, _ = req("GET", "/compras/paginado?eventoId=-2", admin)
check("compras admin por evento con nombres", st == 200 and b["totalRegistros"] == 3 and b["items"][0]["eventoNombre"], b)

print("== Seguridad")
st, _, _ = req("GET", "/usuarios/paginado", lucia)
check("cliente NO ve usuarios/DNI ajenos (403)", st == 403, st)
st, _, _ = req("GET", "/compras/usuario/-3", lucia)
check("cliente NO ve compras de otro (403)", st == 403, st)
st, _, _ = req("GET", "/compras/usuario/-2", lucia)
check("cliente ve sus compras", st == 200, st)
st, _, _ = req("GET", "/compras/-4", lucia)
check("cliente NO ve compra ajena por id (403)", st == 403, st)
st, _, _ = req("DELETE", "/compras/-4", lucia)
check("cliente NO anula compras (403)", st == 403, st)
st, _, _ = req("GET", "/reportes/ventas", lucia)
check("cliente NO ve reportes (403)", st == 403, st)
st, _, _ = req("GET", "/reportes/ventas")
check("anonimo NO ve reportes (401)", st == 401, st)
st, _, _ = req("GET", "/entradas/evento/-2", lucia)
check("cliente NO lista entradas de un evento (403)", st == 403, st)
st, b, _ = req("POST", "/compras", lucia, {"usuarioId": -3, "eventoId": -2, "sectorId": -4, "cantidad": 1, "metodoPago": "Directo / Prueba", "estadoPago": "Aprobado"})
check("compra con UsuarioId ajeno queda a nombre del token", st == 201 and b["usuarioId"] == -2, (st, b))

print("== Mis Entradas")
st, b, _ = req("GET", "/compras/mis-entradas", lucia)
check("mis-entradas sin parametros", st == 200, (st, b))
ids = [p["eventoId"] for p in b["items"]]
check("cada partido una sola vez", len(ids) == len(set(ids)), ids)
grupo = next(p for p in b["items"] if p["eventoId"] == -2)
check("varias compras del mismo partido agrupadas", grupo["cantidadCompras"] == 3 and grupo["cantidadEntradas"] == 5, grupo)
check("total pagado sin duplicar", grupo["totalPagado"] == 54000 + 9000 + 9000, grupo["totalPagado"])
check("sin fila/asiento en Mis Entradas", "filaAsiento" not in json.dumps(b), "")
check("QR por entrada", all(e["codigoQr"] for e in grupo["entradas"]))
check("orden por defecto: proximo primero, pasado al final", b["items"][0]["eventoId"] == -2 and b["items"][-1]["eventoId"] == -3, ids)
check("evento pasado sigue visible como Finalizado", b["items"][-1]["estado"] == "Finalizado", b["items"][-1]["estado"])
st, b, _ = req("GET", "/compras/mis-entradas?estado=pasados", lucia)
check("filtro pasados", st == 200 and [p["eventoId"] for p in b["items"]] == [-3], b)
st, b, _ = req("GET", "/compras/mis-entradas?tamanoPagina=1", lucia)
check("paginacion por partido (2 partidos -> 2 paginas)", b["totalRegistros"] == 2 and b["totalPaginas"] == 2 and len(b["items"]) == 1, b)
st, b, _ = req("GET", "/compras/mis-entradas?ordenarPor=compra_desc", lucia)
check("orden por compra mas reciente", st == 200 and b["items"][0]["eventoId"] == -2, b)
st, b, _ = req("GET", "/compras/mis-entradas?busqueda=boca", lucia)
check("busqueda por club", st == 200 and [p["eventoId"] for p in b["items"]] == [-2], b)
st, b, _ = req("GET", "/compras/-2", lucia)
check("fila/asiento se conservan en los datos", st == 200 and sorted(e["filaAsiento"] for e in b["entradas"]) == ["Fila 1, Asiento 1", "Fila 1, Asiento 2", "Fila 1, Asiento 3"], b)

print("== DNI y usuarios")
nuevo = {"nombre": "Ana", "apellido": "Lopez", "email": "ana.lopez@mail.com", "pass": "secreto1", "rol": "Cliente", "dni": "32.555.666", "telefono": "341 444-5555"}
st, b, _ = req("POST", "/auth/register", cuerpo=nuevo)
check("registro con DNI valido", st == 200, (st, b))
st, b, _ = req("POST", "/auth/register", cuerpo={**nuevo, "email": "otra@mail.com"})
check("rechazo DNI duplicado", st == 400 and "DNI" in json.dumps(b), (st, b))
st, b, _ = req("POST", "/auth/register", cuerpo={**nuevo, "email": "x1@mail.com", "dni": "12AB"})
check("rechazo DNI invalido", st == 400, (st, b))
st, b, _ = req("POST", "/auth/register", cuerpo={**nuevo, "email": "x2@mail.com", "dni": ""})
check("rechazo registro sin DNI", st == 400, (st, b))
st, b, _ = req("GET", "/usuarios/paginado?dni=32555666", admin)
check("DNI guardado normalizado", b["items"][0]["dni"] == "32555666", b)
ana_id = b["items"][0]["idUsuario"]
st, b, _ = req("PUT", "/usuarios", admin, {"idUsuario": ana_id, "nombre": "Ana", "apellido": "Lopez", "email": "ana.lopez@mail.com", "rol": "Cliente", "dni": "38456789"})
check("edicion con DNI de otro -> 409", st == 409, (st, b))
st, b, _ = req("PUT", "/usuarios", admin, {"idUsuario": ana_id, "nombre": "Ana Maria", "apellido": "Lopez", "email": "ana.lopez@mail.com", "rol": "Cliente"})
check("edicion sin DNI conserva el actual", st == 204 and req("GET", f"/usuarios/{ana_id}", admin)[1]["dni"] == "32555666", st)
ana, _ = login("ana.lopez@mail.com", "secreto1")
check("login usuario nuevo", bool(ana))

print("== Reportes")
st, r, _ = req("GET", "/reportes/ventas", admin)
check("reporte ventas", st == 200, (st, r))
res = r["resumen"]
check("total recaudado = suma de compras aprobadas (seed 205000 + 9000 compra de prueba)", res["totalRecaudado"] == 214000, res)
check("total por evento = total general", sum(e["recaudado"] for e in r["porEvento"]) == res["totalRecaudado"])
check("total por club (local) = total general (sin doble conteo)", sum(c["recaudado"] for c in r["porClub"]) == res["totalRecaudado"])
check("total por sector = total general", sum(s["recaudado"] for s in r["porSector"]) == res["totalRecaudado"])
check("entradas por periodo = entradas vendidas", sum(p["entradas"] for p in r["porPeriodo"]) == res["entradasVendidas"])
check("4 graficos SVG", len(r["graficos"]) == 4 and all(g["svg"].startswith("<svg") for g in r["graficos"]))
st, r2, _ = req("GET", "/reportes/ventas?desde=2026-02-01&hasta=2026-02-28", admin)
check("filtro por periodo (febrero: 2 compras, 90000)", r2["resumen"]["comprasConfirmadas"] == 2 and r2["resumen"]["totalRecaudado"] == 90000, r2["resumen"])
st, r3, _ = req("GET", "/reportes/ventas?eventoId=-3", admin)
check("filtro por evento", r3["resumen"]["totalRecaudado"] == 46000 and r3["eventoNombre"], r3["resumen"])
st, pdf, h = req("GET", "/reportes/ventas/pdf", admin, crudo=True)
check("PDF generado", st == 200 and pdf[:4] == b"%PDF" and len(pdf) > 5000, (st, len(pdf) if isinstance(pdf, bytes) else pdf))
open(f"{SALIDA}/reporte.pdf", "wb").write(pdf)
st, xls, h = req("GET", "/reportes/ventas/xlsx", admin, crudo=True)
check("XLSX generado", st == 200 and xls[:2] == b"PK" and len(xls) > 3000, st)
open(f"{SALIDA}/reporte.xlsx", "wb").write(xls)

print(f"\nRESULTADO: {ok} OK, {fallas} FAIL")
sys.exit(1 if fallas else 0)
