<?php

namespace Tests\Feature;

use App\Models\Order;

class OrderApiTest extends ApiTestCase
{
    public function test_authenticated_user_can_list_orders(): void
    {
        $user = $this->authenticate();
        Order::create(['user_id' => $user->id]);

        $response = $this->getJson('/api/orders');

        $response
            ->assertOk()
            ->assertJsonPath('success', true)
            ->assertJsonCount(1, 'data');
    }

    public function test_authenticated_user_can_show_order(): void
    {
        $user = $this->authenticate();
        $order = Order::create(['user_id' => $user->id]);

        $response = $this->getJson('/api/orders/'.$order->id);

        $response
            ->assertOk()
            ->assertJsonPath('data.id', $order->id);
    }

    public function test_authenticated_user_can_create_order_and_decrease_inventory(): void
    {
        $user = $this->authenticate();
        $bun = $this->createIngredientWithInventory('Bun', 'db', 10, 2);
        $meat = $this->createIngredientWithInventory('Meat', 'kg', 5, 1);

        $product = $this->createProductWithIngredients([
            ['ingredient' => $bun, 'quantity' => 1],
            ['ingredient' => $meat, 'quantity' => 0.2],
        ], [
            'name' => 'Burger',
            'price' => 2000,
        ]);

        $response = $this->postJson('/api/orders', [
            'items' => [
                ['product_id' => $product->id, 'quantity' => 2],
            ],
        ]);

        $response
            ->assertStatus(201)
            ->assertJsonPath('success', true)
            ->assertJsonPath('data.user_id', $user->id)
            ->assertJsonPath('data.items.0.product_id', $product->id);

        $this->assertDatabaseHas('orders', [
            'user_id' => $user->id,
            'status' => 'completed',
        ]);

        $this->assertDatabaseHas('order_items', [
            'product_id' => $product->id,
            'quantity' => 2,
            'price' => 2000,
        ]);

        $this->assertDatabaseHas('inventories', [
            'ingredient_id' => $bun->id,
            'quantity' => 8,
        ]);
        $this->assertDatabaseHas('inventories', [
            'ingredient_id' => $meat->id,
            'quantity' => 4.6,
        ]);

        $this->assertDatabaseHas('stock_movements', [
            'ingredient_id' => $bun->id,
            'change_amount' => -2,
            'reason' => 'sale',
        ]);
        $this->assertDatabaseHas('stock_movements', [
            'ingredient_id' => $meat->id,
            'change_amount' => -0.4,
            'reason' => 'sale',
        ]);
    }

    public function test_order_creation_fails_when_stock_is_not_enough_and_rolls_back(): void
    {
        $this->authenticate();
        $bun = $this->createIngredientWithInventory('Tiny Bun', 'db', 1, 1);
        $product = $this->createProductWithIngredients([
            ['ingredient' => $bun, 'quantity' => 2],
        ], [
            'name' => 'Huge Burger',
            'price' => 2500,
        ]);

        $response = $this->postJson('/api/orders', [
            'items' => [
                ['product_id' => $product->id, 'quantity' => 1],
            ],
        ]);

        $response
            ->assertStatus(400)
            ->assertJsonPath('success', false);

        $this->assertDatabaseCount('orders', 0);
        $this->assertDatabaseCount('order_items', 0);
        $this->assertDatabaseHas('inventories', [
            'ingredient_id' => $bun->id,
            'quantity' => 1,
        ]);
    }

    public function test_order_creation_validates_payload(): void
    {
        $this->authenticate();

        $response = $this->postJson('/api/orders', [
            'items' => [],
        ]);

        $response
            ->assertStatus(422)
            ->assertJsonValidationErrors(['items']);
    }

    public function test_authenticated_user_can_update_order_status(): void
    {
        $user = $this->authenticate();
        $order = Order::create(['user_id' => $user->id]);

        $response = $this->patchJson('/api/orders/'.$order->id.'/status', [
            'status' => 'cancelled',
        ]);

        $response
            ->assertOk()
            ->assertJsonPath('success', true)
            ->assertJsonPath('data.status', 'cancelled');

        $this->assertDatabaseHas('orders', [
            'id' => $order->id,
            'status' => 'cancelled',
        ]);
    }

    public function test_update_status_validates_status_field(): void
    {
        $user = $this->authenticate();
        $order = Order::create(['user_id' => $user->id]);

        $response = $this->patchJson('/api/orders/'.$order->id.'/status', []);

        $response
            ->assertStatus(422)
            ->assertJsonValidationErrors(['status']);
    }

    public function test_order_routes_require_authentication(): void
    {
        $this->getJson('/api/orders')->assertStatus(401);
        $this->postJson('/api/orders', [])->assertStatus(401);
        $this->getJson('/api/orders/1')->assertStatus(401);
        $this->patchJson('/api/orders/1/status', [])->assertStatus(401);
    }
}
