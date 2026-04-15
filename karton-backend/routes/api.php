<?php

use Illuminate\Http\Request;
use Illuminate\Support\Facades\Route;

use App\Http\Controllers\Api\AuthController;
use App\Http\Controllers\Api\ProductController;
use App\Http\Controllers\Api\IngredientController;
use App\Http\Controllers\Api\InventoryController;
use App\Http\Controllers\Api\OrderController;
use App\Http\Controllers\Api\SupplierController;
use App\Http\Controllers\Api\PurchaseOrderController;
use App\Http\Controllers\Api\AnalyticsController;

Route::options('{any}', function () {
    return response('', 204);
})->where('any', '.*');

Route::prefix('auth')->group(function () {
    Route::post('/login', [AuthController::class, 'login']);
});
Route::post('/register', [AuthController::class, 'register']); 

Route::get('/test-ingredients', [IngredientController::class, 'index']);

Route::middleware('auth:sanctum')->group(function () {

    Route::post('/auth/logout', [AuthController::class, 'logout']);
    Route::get('/auth/me', [AuthController::class, 'me']);

    // Products
    Route::apiResource('products', ProductController::class);

    // Ingredients
    Route::apiResource('ingredients', IngredientController::class);

    // Inventory
    Route::prefix('inventory')->group(function () {
        Route::get('/', [InventoryController::class, 'index']);
        Route::post('/{ingredient_id}/adjust', [InventoryController::class, 'update']);
        Route::get('/low-stock', [InventoryController::class, 'lowStock']);
        Route::post('/restock', [InventoryController::class, 'restock']);
    });

    // Orders
    Route::apiResource('orders', OrderController::class)->only([
        'index', 'store', 'show'
    ]);
    Route::patch('orders/{id}/status', [OrderController::class, 'updateStatus']);

    // Suppliers
    Route::apiResource('suppliers', SupplierController::class);

    // Purchase Orders
    Route::apiResource('purchase-orders', PurchaseOrderController::class)->only([
        'index', 'store', 'show'
    ]);
    Route::post('purchase-orders/{id}/receive', [PurchaseOrderController::class, 'receive']);

    // Analytics
    Route::prefix('analytics')->group(function () {
        Route::get('reorder-suggestions', [AnalyticsController::class, 'reorderSuggestion']);
        Route::get('low-stock-summary', [AnalyticsController::class, 'lowStockSummary']);
        Route::get('top-products', [AnalyticsController::class, 'topProducts']);
        Route::get('ingredient-usage', [AnalyticsController::class, 'ingredientUsage']);
    });
});
