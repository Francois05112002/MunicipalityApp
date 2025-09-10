// site.js

document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('issues-form');
    if (!form) return;

    const fields = [
        form.querySelector('[name="Location"]'),
        form.querySelector('[name="Category"]'),
        form.querySelector('[name="Description"]')
    ];
    const progressBar = document.getElementById('form-progress-bar');

    function updateProgress() {
        let filled = 0;
        if (fields[0] && fields[0].value.trim() !== '') filled++;
        if (fields[1] && fields[1].value) filled++;
        if (fields[2] && fields[2].value.trim() !== '') filled++;
        // Image is optional, count if a file is selected
        //if (fields[3] && fields[3].value) filled++;

        const percent = (filled / fields.length) * 100;
        progressBar.style.width = percent + '%';
    }

    fields.forEach(f => f && f.addEventListener('input', updateProgress));
    updateProgress();
});

function showComingSoon() {
    document.getElementById('comingSoonModal').style.display = 'flex';
}
function closeComingSoon() {
    document.getElementById('comingSoonModal').style.display = 'none';
}

