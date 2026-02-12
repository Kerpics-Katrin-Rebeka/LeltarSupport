CREATE DATABASE inventory;
USE inventory;


CREATE TABLE roles (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE
);


CREATE TABLE users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(100) UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE user_roles (
    user_id INT,
    role_id INT,
    PRIMARY KEY (user_id, role_id),
    FOREIGN KEY (user_id) REFERENCES users(id),
    FOREIGN KEY (role_id) REFERENCES roles(id)
);


CREATE TABLE products (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    price DECIMAL(10,2),
    active BOOLEAN DEFAULT TRUE
);



CREATE TABLE ingredients (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    unit VARCHAR(20) NOT NULL 
);


CREATE TABLE product_ingredients (
    product_id INT,
    ingredient_id INT,
    quantity DECIMAL(10,2) NOT NULL,
    PRIMARY KEY (product_id, ingredient_id),
    FOREIGN KEY (product_id) REFERENCES products(id),
    FOREIGN KEY (ingredient_id) REFERENCES ingredients(id)
);


CREATE TABLE inventory (
    ingredient_id INT PRIMARY KEY,
    quantity DECIMAL(10,2) NOT NULL,
    minimum_level DECIMAL(10,2) DEFAULT 0,
    FOREIGN KEY (ingredient_id) REFERENCES ingredients(id)
);


CREATE TABLE stock_movements (
    id INT AUTO_INCREMENT PRIMARY KEY,
    ingredient_id INT,
    change_amount DECIMAL(10,2),
    reason ENUM('order','restock','manual','correction'),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (ingredient_id) REFERENCES ingredients(id)
);


CREATE TABLE orders (
    id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT,
    total_price DECIMAL(10,2),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (user_id) REFERENCES users(id)
);

CREATE TABLE order_items (
    id INT AUTO_INCREMENT PRIMARY KEY,
    order_id INT,
    product_id INT,
    quantity INT NOT NULL,
    FOREIGN KEY (order_id) REFERENCES orders(id),
    FOREIGN KEY (product_id) REFERENCES products(id)
);


CREATE TABLE suppliers (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100),
    contact VARCHAR(100)
);


CREATE TABLE purchase_orders (
    id INT AUTO_INCREMENT PRIMARY KEY,
    supplier_id INT,
    status ENUM('recommended','ordered','received'),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (supplier_id) REFERENCES suppliers(id)
);

CREATE TABLE purchase_order_items (
    id INT AUTO_INCREMENT PRIMARY KEY,
    purchase_order_id INT,
    ingredient_id INT,
    quantity DECIMAL(10,2),
    FOREIGN KEY (purchase_order_id) REFERENCES purchase_orders(id),
    FOREIGN KEY (ingredient_id) REFERENCES ingredients(id)
);
