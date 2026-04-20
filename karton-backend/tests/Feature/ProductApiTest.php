<?php

namespace Tests\Feature;

use App\Models\Ingredient;
use App\Models\Product;

class ProductApiTest extends ApiTestCase
{
    public function test_authenticated_user_can_list_products_with_ingredients(): void
    {
        $this->authenticate();
        $ingredient = Ingredient::create(['name' => 'Bread', 'unit' => 'db']);
        $product = Product::create(['name' => 'Toast', 'price' => 990, 'active' => true]);
        $product->ingredients()->attach($ingredient->id, ['quantity' => 2]);

        $response = $this->getJson('/api/products');

        $response
            ->assertOk()
            ->assertJsonPath('success', true)
            ->assertJsonCount(1, 'data')
            ->assertJsonPath('data.0.ingredients.0.id', $ingredient->id);
    }

    public function test_authenticated_user_can_create_product_with_ingredients(): void
    {
        $this->authenticate();
        $ingredient1 = Ingredient::create(['name' => 'Bun', 'unit' => 'db']);
        $ingredient2 = Ingredient::create(['name' => 'Patty', 'unit' => 'kg']);

        $response = $this->postJson('/api/products', [
            'name' => 'Burger',
            'price' => 1990,
            'active' => true,
            'ingredients' => [
                ['id' => $ingredient1->id, 'quantity' => 1],
                ['id' => $ingredient2->id, 'quantity' => 0.2],
            ],
        ]);

        $response
            ->assertStatus(201)
            ->assertJsonPath('success', true)
            ->assertJsonPath('data.name', 'Burger')
            ->assertJsonCount(2, 'data.ingredients');

        $this->assertDatabaseHas('products', ['name' => 'Burger']);
        $this->assertDatabaseHas('product_ingredient', [
            'product_id' => $response->json('data.id'),
            'ingredient_id' => $ingredient1->id,
        ]);
    }

    public function test_create_product_validates_payload(): void
    {
        $this->authenticate();

        $response = $this->postJson('/api/products', [
            'name' => '',
            'price' => -50,
            'ingredients' => [],
        ]);

        $response
            ->assertStatus(422)
            ->assertJsonValidationErrors(['name', 'price', 'ingredients']);
    }

    public function test_authenticated_user_can_show_product(): void
    {
        $this->authenticate();
        $ingredient = Ingredient::create(['name' => 'Cheese', 'unit' => 'kg']);
        $product = Product::create(['name' => 'Cheese Toast', 'price' => 1200, 'active' => true]);
        $product->ingredients()->attach($ingredient->id, ['quantity' => 0.1]);

        $response = $this->getJson('/api/products/'.$product->id);

        $response
            ->assertOk()
            ->assertJsonPath('data.id', $product->id)
            ->assertJsonPath('data.ingredients.0.id', $ingredient->id);
    }

    public function test_authenticated_user_can_update_product_and_sync_ingredients(): void
    {
        $this->authenticate();
        $ingredient1 = Ingredient::create(['name' => 'Chicken', 'unit' => 'kg']);
        $ingredient2 = Ingredient::create(['name' => 'Wrap', 'unit' => 'db']);
        $ingredient3 = Ingredient::create(['name' => 'Sauce', 'unit' => 'l']);

        $product = Product::create(['name' => 'Chicken Wrap', 'price' => 2200, 'active' => true]);
        $product->ingredients()->attach($ingredient1->id, ['quantity' => 0.2]);
        $product->ingredients()->attach($ingredient2->id, ['quantity' => 1]);

        $response = $this->putJson('/api/products/'.$product->id, [
            'name' => 'Spicy Chicken Wrap',
            'price' => 2350,
            'active' => false,
            'ingredients' => [
                ['id' => $ingredient1->id, 'quantity' => 0.25],
                ['id' => $ingredient3->id, 'quantity' => 0.05],
            ],
        ]);

        $response
            ->assertOk()
            ->assertJsonPath('data.name', 'Spicy Chicken Wrap')
            ->assertJsonCount(2, 'data.ingredients');

        $this->assertDatabaseHas('products', [
            'id' => $product->id,
            'name' => 'Spicy Chicken Wrap',
            'price' => 2350,
            'active' => 0,
        ]);

        $this->assertDatabaseMissing('product_ingredient', [
            'product_id' => $product->id,
            'ingredient_id' => $ingredient2->id,
        ]);
        $this->assertDatabaseHas('product_ingredient', [
            'product_id' => $product->id,
            'ingredient_id' => $ingredient3->id,
        ]);
    }

    public function test_update_product_validates_unique_name(): void
    {
        $this->authenticate();
        $productA = Product::create(['name' => 'A', 'price' => 1000, 'active' => true]);
        $productB = Product::create(['name' => 'B', 'price' => 1000, 'active' => true]);

        $response = $this->putJson('/api/products/'.$productB->id, [
            'name' => 'A',
            'price' => 1000,
        ]);

        $response
            ->assertStatus(422)
            ->assertJsonValidationErrors(['name']);
    }

    public function test_authenticated_user_can_delete_product_and_detach_ingredients(): void
    {
        $this->authenticate();
        $ingredient = Ingredient::create(['name' => 'Egg', 'unit' => 'db']);
        $product = Product::create(['name' => 'Egg Muffin', 'price' => 1300, 'active' => true]);
        $product->ingredients()->attach($ingredient->id, ['quantity' => 1]);

        $response = $this->deleteJson('/api/products/'.$product->id);

        $response->assertOk()->assertJsonPath('success', true);

        $this->assertDatabaseMissing('products', ['id' => $product->id]);
        $this->assertDatabaseMissing('product_ingredient', [
            'product_id' => $product->id,
            'ingredient_id' => $ingredient->id,
        ]);
    }

    public function test_product_routes_require_authentication(): void
    {
        $this->getJson('/api/products')->assertStatus(401);
        $this->postJson('/api/products', [])->assertStatus(401);
        $this->getJson('/api/products/1')->assertStatus(401);
        $this->putJson('/api/products/1', [])->assertStatus(401);
        $this->deleteJson('/api/products/1')->assertStatus(401);
    }
}
