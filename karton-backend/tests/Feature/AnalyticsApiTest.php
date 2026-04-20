<?php

namespace Tests\Feature;

use App\Models\Order;
use App\Models\OrderItem;

class AnalyticsApiTest extends ApiTestCase
{
    public function test_reorder_suggestions_return_only_low_stock_items_with_recommended_order_amount(): void
    {
        $this->authenticate();
        $low = $this->createIngredientWithInventory('Low Stock Item', 'kg', 2, 5);
        $this->createIngredientWithInventory('Enough Stock Item', 'kg', 10, 5);

        $response = $this->getJson('/api/analytics/reorder-suggestions');

        $response
            ->assertOk()
            ->assertJsonPath('success', true)
            ->assertJsonCount(1, 'data')
            ->assertJsonPath('data.0.ingredient_id', $low->id)
            ->assertJsonPath('data.0.recommended_order', 8);
    }

    public function test_low_stock_summary_returns_low_stock_count(): void
    {
        $this->authenticate();
        $this->createIngredientWithInventory('Low1', 'kg', 1, 5);
        $this->createIngredientWithInventory('Low2', 'kg', 2, 5);
        $this->createIngredientWithInventory('Ok', 'kg', 10, 5);

        $response = $this->getJson('/api/analytics/low-stock-summary');

        $response
            ->assertOk()
            ->assertJsonPath('success', true)
            ->assertJsonPath('low_stock_count', 2);
    }

    public function test_top_products_returns_best_selling_products(): void
    {
        $user = $this->authenticate();
        $productA = $this->createProductWithIngredients([], ['name' => 'A', 'price' => 1000]);
        $productB = $this->createProductWithIngredients([], ['name' => 'B', 'price' => 1200]);
        $order = Order::create(['user_id' => $user->id]);

        OrderItem::create([
            'order_id' => $order->id,
            'product_id' => $productA->id,
            'quantity' => 10,
            'price' => 1000,
        ]);
        OrderItem::create([
            'order_id' => $order->id,
            'product_id' => $productB->id,
            'quantity' => 3,
            'price' => 1200,
        ]);

        $response = $this->getJson('/api/analytics/top-products');

        $response
            ->assertOk()
            ->assertJsonPath('success', true)
            ->assertJsonPath('data.0.product_id', $productA->id)
            ->assertJsonPath('data.0.total_sold', 10)
            ->assertJsonPath('data.1.product_id', $productB->id);
    }

    public function test_ingredient_usage_returns_total_used_per_ingredient(): void
    {
        $this->authenticate();
        $ingredient = $this->createIngredientWithInventory('Used Ingredient', 'kg', 20, 2);
        $product = $this->createProductWithIngredients([
            ['ingredient' => $ingredient, 'quantity' => 0.5],
        ], ['name' => 'Usage Product', 'price' => 1500]);

        $user = $this->createUser();
        $order = Order::create(['user_id' => $user->id]);
        OrderItem::create([
            'order_id' => $order->id,
            'product_id' => $product->id,
            'quantity' => 4,
            'price' => 1500,
        ]);

        $response = $this->getJson('/api/analytics/ingredient-usage');

        $response
            ->assertOk()
            ->assertJsonPath('success', true)
            ->assertJsonPath('data.0.id', $ingredient->id)
            ->assertJsonPath('data.0.total_used', 2);
    }

    public function test_analytics_routes_require_authentication(): void
    {
        $this->getJson('/api/analytics/reorder-suggestions')->assertStatus(401);
        $this->getJson('/api/analytics/low-stock-summary')->assertStatus(401);
        $this->getJson('/api/analytics/top-products')->assertStatus(401);
        $this->getJson('/api/analytics/ingredient-usage')->assertStatus(401);
    }
}
