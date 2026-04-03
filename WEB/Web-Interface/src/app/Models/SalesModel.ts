export default interface SalesModel{
    
}

export interface ProductModel{
    id:number;
    name:string;
    price:number;
    quantity:number;
    active:boolean;
}

export interface OrderItemModel{
    id:number;
    product:ProductModel;
    quantity:number;
}

export interface OrderModel{
    id:number;
    price:number;
    date:Date;
    items:OrderItemModel[];
}