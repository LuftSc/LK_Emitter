import { Role } from "../services/usersService";
import { Emitter } from "./Emitter";
import { EmitterInfo } from "./EmitterInfo";
import { PassportInfo, User } from "./User";

export interface UserWithEmitters {
    id: string,
    fullName: string,
    email: string,
    phone: string,
    birthDate: string,
    passport: PassportInfo,
    role: Role
    emitters: Emitter[]
}
/* 
public record DecryptedUser(
    Guid Id,
    string FullName,
    string Email,
    string Phone,
    DateOnly BirthDate,
    DecryptedPassport Passport,
    Role Role,
    List<Emitter>? Emitters
    )
*/


/*Guid Id,
string FullName,
string Email,
string Phone,
//DateOnly BirthDate,
//PassportDTO Passport,
Role Role,
List<EmitterInfoRecord>? Emitters */