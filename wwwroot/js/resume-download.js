document.addEventListener("DOMContentLoaded", () => {
    document.getElementById("resume-download").addEventListener("click", downloadResume);
});

async function downloadResume(event) {
    event.preventDefault();

    const response = await fetch('/download-resume');

    if (response.status === 429) {
        const message = await response.text();
        alert(message);
        return;
    }

    const blob = await response.blob();
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = 'David Bekeris CV - Webbutvecklare.pdf';
    a.click();
    window.URL.revokeObjectURL(url);
}

document.addEventListener("DOMContentLoaded", () => {
    document.getElementById("resume-download").addEventListener("click", downloadResume);
});