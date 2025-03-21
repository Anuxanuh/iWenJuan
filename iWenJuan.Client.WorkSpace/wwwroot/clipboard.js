function copyToClipboard(text) {
    navigator.clipboard.writeText(text)
        .catch(err => {
            alert("复制失败: " + err);
        });
}