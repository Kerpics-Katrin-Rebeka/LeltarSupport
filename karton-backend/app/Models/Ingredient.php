<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class Ingredient extends Model
{
    use HasFactory;

    protected $fillable = [
        'name',
        'unit',
    ];
    
    public function products()
    {
        return $this->belongsToMany(
            Product::class,
            'product_ingredient',
            'ingredient_id',  
            'product_id'      
        )->withPivot('quantity');
    }

    public function inventory()
    {
        return $this->hasOne(Inventory::class);
    }

    public function stockMovements()
    {
        return $this->hasMany(StockMovement::class);
    }
}
