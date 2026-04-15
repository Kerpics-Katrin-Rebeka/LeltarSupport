export default interface IngredientModel{
    id:number,
    name:string,
    minAmount:number,
    unit:string
}

export interface IngredientResponseModel{
    data:IngredientModel[];
    success:boolean;
}