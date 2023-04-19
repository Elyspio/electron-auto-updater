export function openPage(url: string) {
	return window.open(url);
}

export function download(filename: string, data: ArrayBuffer) {
	const url = window.URL.createObjectURL(new Blob([data]));
	const link = document.createElement("a");
	link.href = url;
	link.setAttribute("download", filename); //or any other extension
	document.body.appendChild(link);
	link.click();
	document.body.removeChild(link);
}
