-- Seed data converted from InventorySqlSeeder.php
-- Target schema: invenotry_database.sql

START TRANSACTION;

SET FOREIGN_KEY_CHECKS = 0;

DELETE FROM purchase_order_items;
DELETE FROM purchase_orders;
DELETE FROM order_items;
DELETE FROM orders;
DELETE FROM stock_movements;
DELETE FROM inventory;
DELETE FROM product_ingredients;
DELETE FROM user_roles;
DELETE FROM suppliers;
DELETE FROM products;
DELETE FROM ingredients;
DELETE FROM users;
DELETE FROM roles;

SET FOREIGN_KEY_CHECKS = 1;

INSERT INTO roles (id, name) VALUES
    (1, 'admin'),
    (2, 'manager'),
    (3, 'staff');

INSERT INTO users (id, name, email, password_hash, created_at) VALUES
    (1, 'Admin User',   'admin@inventory.local',   '$2y$12$xDILOdyXW59bXB2RhUUNtepXervSrrzEcWYn0gkG8cMH.5nopG32.', NOW()),
    (2, 'Manager User', 'manager@inventory.local', '$2y$12$RpZLJfWiPb0plVf3aM53c.dKddee4V.ZwPs3FjawqrrX1cTq4DzZi', NOW()),
    (3, 'Staff User',   'staff@inventory.local',   '$2y$12$i5AIsneNL.UaNNNN/Cw0ouFz8cd2gWji0lbtbqMbJkD7LOGjSMyEK', NOW());

INSERT INTO user_roles (user_id, role_id) VALUES
    (1, 1),
    (2, 2),
    (3, 3);

INSERT INTO suppliers (id, name, contact) VALUES
    (1, 'Fresh Farm Kft', 'fresh@farm.hu'),
    (2, 'Daily Dairy Bt', 'sales@dailydairy.hu');

INSERT INTO ingredients (id, name, unit) VALUES
    (1, 'Flour', 'kg'),
    (2, 'Milk', 'l'),
    (3, 'Sugar', 'kg'),
    (4, 'Egg', 'pcs');

INSERT INTO inventory (ingredient_id, quantity, minimum_level) VALUES
    (1, 30.00, 10.00),
    (2, 20.00, 8.00),
    (3, 12.00, 5.00),
    (4, 120.00, 50.00);

INSERT INTO products (id, name, price, active) VALUES
    (1, 'Pancake', 1500.00, 1),
    (2, 'Waffle', 1800.00, 1),
    (3, 'Crepe', 1700.00, 1);

INSERT INTO product_ingredients (product_id, ingredient_id, quantity) VALUES
    (1, 1, 0.20),
    (1, 2, 0.10),
    (1, 4, 2.00),
    (2, 1, 0.25),
    (2, 2, 0.12),
    (2, 4, 2.00),
    (3, 1, 0.18),
    (3, 2, 0.10),
    (3, 3, 0.03),
    (3, 4, 1.00);

INSERT INTO stock_movements (ingredient_id, change_amount, reason, created_at) VALUES
    (1, 30.00, 'restock', NOW()),
    (2, 20.00, 'restock', NOW()),
    (3, 12.00, 'restock', NOW()),
    (4, 120.00, 'restock', NOW());

INSERT INTO orders (id, user_id, total_price, created_at) VALUES
    (1, 3, 3300.00, NOW());

INSERT INTO order_items (order_id, product_id, quantity) VALUES
    (1, 1, 1),
    (1, 2, 1);

INSERT INTO purchase_orders (id, supplier_id, status, created_at) VALUES
    (1, 1, 'recommended', NOW());

INSERT INTO purchase_order_items (purchase_order_id, ingredient_id, quantity) VALUES
    (1, 1, 25.00),
    (1, 2, 15.00);

COMMIT;
