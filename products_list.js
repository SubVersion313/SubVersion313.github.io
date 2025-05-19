// products_list.js
document.addEventListener('DOMContentLoaded', () => {
    // Get all products
    const allProducts = Object.values(window.products).flat();
    let filteredProducts = [...allProducts];
    
    // DOM elements
    const container = document.getElementById('category-sections');
    const priceValueElement = document.getElementById('priceValue');
    const categoryCheckboxes = document.querySelectorAll('input[name="category"]');

    // Initialize with all categories checked
    document.querySelectorAll('input[name="category"]').forEach(checkbox => {
        checkbox.checked = true;
    });

    // Render function
    function renderProducts() {
        container.innerHTML = '';
        
        // Group products by category
        const categories = filteredProducts.reduce((acc, product) => {
            if (!acc[product.category]) acc[product.category] = [];
            acc[product.category].push(product);
            return acc;
        }, {});

        // Create category sections
        Object.entries(categories).forEach(([category, products]) => {
            const section = document.createElement('div');
            section.className = 'category-section';
            section.id = category;
            
            // Create header
            const header = document.createElement('h2');
            header.className = 'category-header';
            header.textContent = formatCategoryName(category);
            
            // Create items container
            const itemsContainer = document.createElement('div');
            itemsContainer.className = 'items';
            
            // Append products
            products.forEach(product => {
                itemsContainer.appendChild(createProductElement(product));
            });
            
            // Build section
            section.appendChild(header);
            section.appendChild(itemsContainer);
            container.appendChild(section);
        });
    }

    // Create individual product element
    function createProductElement(product) {
        const element = document.createElement('div');
        element.className = 'item';
        
        const htmlContent = `
            <img src="${product.img}" alt="${product.name}">
            <div class="info">
                <h3>${product.name}</h3>
                <p class="price">$${product.price.toFixed(2)}</p>
                <div class="product-controls">
                    <div class="quantity-selector">
                        <label>Qty:</label>
                        <input type="number" class="quantity-input" min="1" value="1">
                    </div>
                    <div class="product-buttons">
                        <button onclick="addToCart('${product.category}', ${product.id})">Add to Cart</button>
                    </div>
                </div>
            </div>
        `;
        
        element.innerHTML = htmlContent;
        return element;
    }

    // Format category name for display
    function formatCategoryName(category) {
        return category
            .replace(/([A-Z])/g, ' $1')
            .toUpperCase();
    }

    // Filter functions
    function updateFilters() {
        const selectedCategories = Array.from(document.querySelectorAll('input[name="category"]:checked'))
            .map(checkbox => checkbox.value);
        
        const maxPrice = parseFloat(document.getElementById('priceRange').value);
        
        filteredProducts = allProducts.filter(product => {
            const categoryMatch = selectedCategories.includes(product.category);
            const priceMatch = product.price <= maxPrice;
            return categoryMatch && priceMatch;
        });
        
        renderProducts();
    }

    // Event listeners
    categoryCheckboxes.forEach(checkbox => {
        checkbox.addEventListener('change', updateFilters);
    });

    document.getElementById('priceRange').addEventListener('input', function() {
        priceValueElement.textContent = this.value;
        updateFilters();
    });

    // Initial render
    renderProducts();
});

// Cart functions (reuse from home.js)
function addToCart(category, productId) {
    const itemElement = document.querySelector(`[data-category="${category}"][data-id="${productId}"]`);
    const quantityInput = itemElement?.querySelector('.quantity-input');
    const quantity = parseInt(quantityInput?.value) || 1;
    
    const product = window.products[category].find(item => item.id === productId);
    
    if (product) {
        const existingItem = window.cart.find(item => 
            item.id === productId && item.category === category
        );
        
        if (existingItem) {
            existingItem.quantity += quantity;
        } else {
            window.cart.push({
                ...product,
                quantity: quantity
            });
        }
        
        updateCart();
        showNotification(`${quantity} ${product.name} added to cart!`);
        if (quantityInput) quantityInput.value = 1;
    }
}

function updateCart() {
    const cartContainer = document.getElementById('cart-items');
    const totalPriceElement = document.getElementById('total-price');
    
    cartContainer.innerHTML = '';
    
    cart.forEach((item, index) => {
        const cartItem = document.createElement('div');
        cartItem.className = 'cart-item';
        cartItem.innerHTML = `
            <p>${item.name} (Qty: ${item.quantity}) - $${(item.price * item.quantity).toFixed(2)}</p>
            <button onclick="removeFromCart(${index})">Remove</button>
        `;
        cartContainer.appendChild(cartItem);
    });

    const totalPrice = cart.reduce((total, item) => total + (item.price * item.quantity), 0);
    totalPriceElement.textContent = totalPrice.toFixed(2);
}

function showNotification(message) {
    const notification = document.createElement('div');
    notification.className = 'cart-notification';
    notification.textContent = message;
    document.body.appendChild(notification);
    setTimeout(() => notification.remove(), 2000);
}

function scrollToCategory(category) {
    if (!category) return;
    
    const section = document.getElementById(category);
    if (section) {
        section.scrollIntoView({
            behavior: 'smooth',
            block: 'start'
        });
    } else {
        alert('This category is currently filtered out or unavailable');
    }
}

document.querySelectorAll('input[name="category"]').forEach(checkbox => {
    checkbox.addEventListener('change', updateFilters);
});

document.getElementById('priceRange').addEventListener('input', function() {
    document.getElementById('priceValue').textContent = this.value;
    updateFilters();
});