import { Role } from "../services/usersService"

export interface Document {
    id : string,
    role: Role
    title : string,
    fileExtnsion : string,
    uploadDate : string,
    size : string
}