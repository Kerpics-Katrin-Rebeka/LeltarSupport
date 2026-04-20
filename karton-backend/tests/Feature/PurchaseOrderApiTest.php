<?php

namespace Tests\Feature;

use App\Models\PurchaseOrder;
use App\Models\PurchaseOrderItem;

class PurchaseOrderApiTest extends ApiTestCase
{
    public function test_authenticated_user_can_list_purchase_orders(): void
    {
        $this->authenticate();
        $supplier = $this->createSupplier();
        PurchaseOrder::create(['supplier_id' => $supplier->id, 'status' => 'pending']);

        $response = $this->getJson('/api/purchase-orders');

        $response
            ->assertOk()
            ->assertJsonPath('success', true)
            ->assertJsonCount(1, 'data');
    }

    public function test_authenticated_user_can_show_purchase_order(): void
    {
        $this->authenticate();
        $supplier = $this->createSupplier();
        $order = PurchaseOrder::create(['supplier_id' => $supplier->id, 'status' => 'pending']);

        $response = $this->getJson('/api/purchase-orders/'.$order->id);

        $response
            ->assertOk()
            ->assertJsonPath('data.id', $order->id);
    }

    public function test_authenticated_user_can_create_purchase_order(): void
    {
        $this->authenticate();
        $supplier = $this->createSupplier();
        $ingredient = $this->createIngredientWithInventory('Potato', 'kg', 3, 1);

        $response = $this->postJson('/api/purchase-orders', [
            'supplier_id' => $supplier->id,
            'items' => [
                ['ingredient_id' => $ingredient->id, 'quantity' => 5],
            ],
        ]);

        $response
            ->assertStatus(201)
            ->assertJsonPath('success', true)
            ->assertJsonPath('data.supplier_id', $supplier->id)
            ->assertJsonCount(1, 'data.items');

        $this->assertDatabaseHas('purchase_orders', [
            'supplier_id' => $supplier->id,
            'status' => 'pending',
        ]);
        $this->assertDatabaseHas('purchase_order_items', [
            'ingredient_id' => $ingredient->id,
            'quantity' => 5,
        ]);
    }

    public function test_create_purchase_order_validates_payload(): void
    {
        $this->authenticate();

        $response = $this->postJson('/api/purchase-orders', [
            'supplier_id' => 999,
            'items' => [],
        ]);

        $response
            ->assertStatus(422)
            ->assertJsonValidationErrors(['supplier_id', 'items']);
    }

    public function test_authenticated_user_can_receive_purchase_order_and_increase_stock(): void
    {
        $this->authenticate();
        $supplier = $this->createSupplier();
        $ingredient = $this->createIngredientWithInventory('Tomato', 'kg', 4, 1);

        $order = PurchaseOrder::create([
            'supplier_id' => $supplier->id,
            'status' => 'pending',
        ]);

        PurchaseOrderItem::create([
            'purchase_order_id' => $order->id,
            'ingredient_id' => $ingredient->id,
            'quantity' => 6,
        ]);

        $response = $this->postJson('/api/purchase-orders/'.$order->id.'/receive');

        $response
            ->assertOk()
            ->assertJsonPath('success', true)
            ->assertJsonPath('data.status', 'received');

        $this->assertDatabaseHas('inventories', [
            'ingredient_id' => $ingredient->id,
            'quantity' => 10,
        ]);

        $this->assertDatabaseHas('stock_movements', [
            'ingredient_id' => $ingredient->id,
            'change_amount' => 6,
            'reason' => 'purchase',
        ]);
    }

    public function test_receive_purchase_order_fails_if_already_received(): void
    {
        $this->authenticate();
        $supplier = $this->createSupplier();
        $order = PurchaseOrder::create([
            'supplier_id' => $supplier->id,
            'status' => 'received',
        ]);

        $response = $this->postJson('/api/purchase-orders/'.$order->id.'/receive');

        $response
            ->assertStatus(400)
            ->assertJsonPath('success', false)
            ->assertJsonPath('message', 'Already received');
    }

    public function test_receive_purchase_order_fails_if_inventory_record_is_missing(): void
    {
        $this->authenticate();
        $supplier = $this->createSupplier();
        $ingredient = $this->createIngredientWithInventory('Cucumber', 'kg', 1, 1);
        $ingredient->inventory()->delete();

        $order = PurchaseOrder::create([
            'supplier_id' => $supplier->id,
            'status' => 'pending',
        ]);

        PurchaseOrderItem::create([
            'purchase_order_id' => $order->id,
            'ingredient_id' => $ingredient->id,
            'quantity' => 2,
        ]);

        $response = $this->postJson('/api/purchase-orders/'.$order->id.'/receive');

        $response
            ->assertStatus(400)
            ->assertJsonPath('success', false);

        $this->assertDatabaseHas('purchase_orders', [
            'id' => $order->id,
            'status' => 'pending',
        ]);
    }

    public function test_purchase_order_routes_require_authentication(): void
    {
        $this->getJson('/api/purchase-orders')->assertStatus(401);
        $this->postJson('/api/purchase-orders', [])->assertStatus(401);
        $this->getJson('/api/purchase-orders/1')->assertStatus(401);
        $this->postJson('/api/purchase-orders/1/receive')->assertStatus(401);
    }
}
