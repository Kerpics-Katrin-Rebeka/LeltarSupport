import IngredientModel from "./IngredientModel";

export default interface SalesModel {}

export interface ProductModel{
    id:number;
    name:string;
    price:number;
    quantity:number;
    active:boolean;
}

export interface ItemModel{
    id:number;
    product:ProductModel;
    quantity:number;
}

export interface RecommendationItemModel{
    id:number;
    ingredient:IngredientModel;
    quantity:number;
}

export interface OrderModel{
    id:number;
    price:number;
    created_at:string|Date;
    items:ItemModel[];
}

export interface SupplierModel{
    id:number;
    name:string;
    contact:string;
}

export interface RestockModel{
    id:number;
    supplier_id:number;
    status:string;
    created_at:string|Date;
    items:RecommendationItemModel[];
}

export interface MovementModel{
    id:number;
    ingredient_id:number;
    change_amount:number;
    reason:string;
    created_at:string|Date;
    ingredient:IngredientModel;
}