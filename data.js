{
    "roles": [
        {
            "id": 1,
            "name": "admin"
        },
        {
            "id": 2,
            "name": "manager"
        },
        {
            "id": 3,
            "name": "cashier"
        }
    ],
    "users": [
        {
            "id": 1,
            "name": "Admin User",
            "email": "admin@example.com",
            "password_hash": "$2b$10$examplehash",
            "created_at": "2026-03-05"
        },
        {
            "id": 2,
            "name": "Cashier User",
            "email": "cashier@example.com",
            "password_hash": "$2b$10$examplehash",
            "created_at": "2026-03-05"
        }
    ],
    "user_roles": [
        {
            "user_id": 1,
            "role_id": 1
        },
        {
            "user_id": 2,
            "role_id": 3
        }
    ],
    "products": [
        {
            "id": 1,
            "name": "Margherita Pizza",
            "price": 8.5,
            "active": true
        },
        {
            "id": 2,
            "name": "Pepperoni Pizza",
            "price": 9.5,
            "active": true
        },
        {
            "id": 3,
            "name": "Cheese Burger",
            "price": 7,
            "active": true
        }
    ],
    "ingredients": [
        {
            "id": 1,
            "name": "Flour",
            "unit": "kg"
        },
        {
            "id": 2,
            "name": "Tomato Sauce",
            "unit": "l"
        },
        {
            "id": 3,
            "name": "Cheese",
            "unit": "kg"
        },
        {
            "id": 4,
            "name": "Pepperoni",
            "unit": "kg"
        },
        {
            "id": 5,
            "name": "Burger Bun",
            "unit": "pcs"
        },
        {
            "id": 6,
            "name": "Beef Patty",
            "unit": "pcs"
        }
    ],
    "product_ingredients": [
        {
            "product_id": 1,
            "ingredient_id": 1,
            "quantity": 0.25
        },
        {
            "product_id": 1,
            "ingredient_id": 2,
            "quantity": 0.1
        },
        {
            "product_id": 1,
            "ingredient_id": 3,
            "quantity": 0.2
        },
        {
            "product_id": 2,
            "ingredient_id": 1,
            "quantity": 0.25
        },
        {
            "product_id": 2,
            "ingredient_id": 2,
            "quantity": 0.1
        },
        {
            "product_id": 2,
            "ingredient_id": 3,
            "quantity": 0.2
        },
        {
            "product_id": 2,
            "ingredient_id": 4,
            "quantity": 0.15
        },
        {
            "product_id": 3,
            "ingredient_id": 5,
            "quantity": 1
        },
        {
            "product_id": 3,
            "ingredient_id": 6,
            "quantity": 1
        },
        {
            "product_id": 3,
            "ingredient_id": 3,
            "quantity": 0.05
        }
    ],
    "inventory": [
        {
            "ingredient_id": 1,
            "quantity": 50,
            "minimum_level": 10
        },
        {
            "ingredient_id": 2,
            "quantity": 20,
            "minimum_level": 5
        },
        {
            "ingredient_id": 3,
            "quantity": 15,
            "minimum_level": 5
        },
        {
            "ingredient_id": 4,
            "quantity": 10,
            "minimum_level": 3
        },
        {
            "ingredient_id": 5,
            "quantity": 100,
            "minimum_level": 20
        },
        {
            "ingredient_id": 6,
            "quantity": 50,
            "minimum_level": 10
        }
    ],
    "stock_movements": [
        {
            "id": 1,
            "ingredient_id": 1,
            "change_amount": 20,
            "reason": "restock",
            "created_at": "2026-03-05"
        },
        {
            "id": 2,
            "ingredient_id": 3,
            "change_amount": -1,
            "reason": "order",
            "created_at": "2026-03-05"
        }
    ],
    "orders": [
        {
            "id": 1,
            "user_id": 2,
            "total_price": 18,
            "created_at": "2026-03-05"
        }
    ],
    "order_items": [
        {
            "id": 1,
            "order_id": 1,
            "product_id": 1,
            "quantity": 1
        },
        {
            "id": 2,
            "order_id": 1,
            "product_id": 2,
            "quantity": 1
        }
    ],
    "suppliers": [
        {
            "id": 1,
            "name": "Food Supplier Ltd",
            "contact": "supplier@example.com"
        }
    ],
    "purchase_orders": [
        {
            "id": 1,
            "supplier_id": 1,
            "status": "recommended",
            "created_at": "2026-03-05"
        }
    ],
    "purchase_order_items": [
        {
            "id": 1,
            "purchase_order_id": 1,
            "ingredient_id": 3,
            "quantity": 10
        }
    ]
}