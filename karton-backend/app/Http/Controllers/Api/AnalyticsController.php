<?php

namespace App\Http\Controllers\Api;

use App\Http\Controllers\Controller;
use Illuminate\Http\Request;
use App\Models\Inventory;
use App\Models\OrderItem;
use App\Models\Product;
use App\Models\Ingredient;
use Illuminate\Support\Facades\DB;

class AnalyticsController extends Controller
{
    public function reorderSuggestion()
    {
        $items = Inventory::with('ingredient')
            ->whereColumn('quantity', '<', 'minimum_level')
            ->get()
            ->map(function ($inv) {
                return [
                    'ingredient_id' => $inv->ingredient_id,
                    'ingredient_name' => $inv->ingredient->name,
                    'current_stock' => $inv->quantity,
                    'minimum_level' => $inv->minimum_level,
                    'recommended_order' => ($inv->minimum_level * 2) - $inv->quantity
                ];
            });

        return response()->json([
            'success' => true,
            'data' => $items
        ]);
    }

    public function lowStockSummary()
    {
        $lowStock = Inventory::whereColumn('quantity', '<', 'minimum_level')->count();

        return response()->json([
            'success' => true,
            'low_stock_count' => $lowStock
        ]);
    }

    public function topProducts()
    {
        $products = OrderItem::select(
                'product_id',
                DB::raw('SUM(quantity) as total_sold')
            )
            ->groupBy('product_id')
            ->with('product')
            ->orderByDesc('total_sold')
            ->limit(5)
            ->get()
            ->map(function ($item) {
                return [
                    'product_id' => $item->product_id,
                    'product_name' => $item->product->name,
                    'total_sold' => $item->total_sold
                ];
            });

        return response()->json([
            'success' => true,
            'data' => $products
        ]);
    }

    public function ingredientUsage()
    {
        $usage = DB::table('order_items')
            ->join('products', 'order_items.product_id', '=', 'products.id')
            ->join('product_ingredients', 'products.id', '=', 'product_ingredients.product_id')
            ->join('ingredients', 'ingredients.id', '=', 'product_ingredients.ingredient_id')
            ->select(
                'ingredients.id',
                'ingredients.name',
                DB::raw('SUM(order_items.quantity * product_ingredients.quantity) as total_used')
            )
            ->groupBy('ingredients.id', 'ingredients.name')
            ->orderByDesc('total_used')
            ->get();

        return response()->json([
            'success' => true,
            'data' => $usage
        ]);
    }
}