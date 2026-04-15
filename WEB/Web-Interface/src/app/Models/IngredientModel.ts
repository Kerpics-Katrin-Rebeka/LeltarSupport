export default interface IngredientModel{
    id:number,
    name:string,
    minAmount:number,
    unit:string
}

export interface ResponseModel{
    current_stock:number;
    ingredient_name:string;
    ingredient_id:number;
    minimum_level:number;
    recommended_order:number;
}

export interface UnderLimitResponseModel{
    data:ResponseModel[];
    success:boolean;
}

export interface IngredientResponseModel{
    data:IngredientModel[];
    success:boolean;
}