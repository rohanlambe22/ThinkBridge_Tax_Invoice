
document.addEventListener('DOMContentLoaded', function () {
    const renderInvoice = (data) => {
        if (!data || !Array.isArray(data.items)) {
            document.getElementById('invoice-container').textContent = 'No items to display.';
            return;
        }
        let html = '<ul>';
        data.items.forEach((item) => {
            const price = typeof item.price === 'number' ? item.price.toFixed(2) : item.price;
            html += `<li>${item.name} - $${price}</li>`;
        });
        html += '</ul>';
        document.getElementById('invoice-container').innerHTML = html;
    };

    const mockData = { items: [
        { name: 'Widget A', price: 19.99 },
        { name: 'Widget B', price: 9.5 }
    ]};

    const isGhPages = location.hostname.endsWith('github.io');
    const renderApiUrl = 'https://YOUR-RENDER-SERVICE.onrender.com'; // TODO: replace with your Render URL
    const baseUrl = isGhPages ? renderApiUrl : ((location.protocol === 'file:') ? 'http://localhost:5000' : '');
    fetch(`${baseUrl}/api/invoice`)
        .then((resp) => {
            if (!resp.ok) throw new Error('Network response was not ok');
            return resp.json();
        })
        .then((data) => renderInvoice(data))
        .catch((error) => {
            console.warn('Falling back to mock data:', error.message);
            renderInvoice(mockData);
        });
});
