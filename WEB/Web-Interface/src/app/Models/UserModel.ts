export default interface UserModel {
    name:string,
    email:string,
    token:string
}

export interface response{
  user:UserModel,
  token:string
}