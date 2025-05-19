USE El_Harrifa;
GO

-- Insert sample categories
INSERT INTO Categories (name, description) VALUES
('Fiber Crafts', 'Handmade fiber art and crafts'),
('Handcrafted Toys', 'Unique handmade toys'),
('Paper Goods', 'Handmade paper products'),
('Accessories', 'Handmade accessories'),
('Art Collectibles', 'Unique art pieces'),
('Home Decor', 'Handmade home decoration items');
GO

-- Insert sample admin user
INSERT INTO Users (nameuser, email, passworduser, is_admin) VALUES
('Admin User', 'admin@el-harrifa.com', '$2a$11$YourHashedPasswordHere', 1);
GO

-- Insert sample regular user
INSERT INTO Users (nameuser, email, passworduser, address) VALUES
('Test User', 'test@example.com', '$2a$11$YourHashedPasswordHere', '123 Test Street');
GO

-- Insert sample products
INSERT INTO Products (name, price, image_url, category, details, seller_id) VALUES
('Handmade Scarf', 29.99, 'products/scarf.jpg', 'Fiber Crafts', '{"color": "Blue", "material": "Wool"}', 1),
('Wooden Toy Car', 19.99, 'products/toy-car.jpg', 'Handcrafted Toys', '{"age": "3+", "material": "Wood"}', 1),
('Decorative Card', 4.99, 'products/card.jpg', 'Paper Goods', '{"occasion": "Birthday", "style": "Vintage"}', 1);
GO

-- Insert sample review
INSERT INTO Reviews (user_id, product_id, rating, comment) VALUES
(2, 1, 5, 'Beautiful quality!');
GO 