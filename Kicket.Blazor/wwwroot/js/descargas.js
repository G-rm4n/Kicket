// Descarga un archivo generado por la API (PDF o XLSX).
// Blazor Server manda los bytes como stream (DotNetStreamReference) y acá se arma un link temporal.
window.kicketDescargar = async function (nombreArchivo, tipoContenido, streamRef) {
    const buffer = await streamRef.arrayBuffer();
    const blob = new Blob([buffer], { type: tipoContenido });
    const url = URL.createObjectURL(blob);
    const link = document.createElement("a");
    link.href = url;
    link.download = nombreArchivo;
    document.body.appendChild(link);
    link.click();
    link.remove();
    URL.revokeObjectURL(url);
};
