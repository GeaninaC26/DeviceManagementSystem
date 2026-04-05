import { Dto } from "./dto";
import { RoleEnum } from "./enums/role.enum";

export class UserDto extends Dto {
  name: string;
  role: RoleEnum;
  location: string;
  email: string;
}
