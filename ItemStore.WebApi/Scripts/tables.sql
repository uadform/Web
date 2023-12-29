CREATE TABLE Shop (
    shop_id SERIAL PRIMARY KEY,
    name VARCHAR(255),
    address VARCHAR(255)
);

CREATE TABLE Item (
    item_id SERIAL PRIMARY KEY,
    name VARCHAR(255),
    price DECIMAL,
    shop_id INTEGER REFERENCES Shop(shop_id) ON DELETE SET NULL
);

CREATE TABLE User_ (
    user_id SERIAL PRIMARY KEY,
    username VARCHAR(255),
    email VARCHAR(255) UNIQUE
);

