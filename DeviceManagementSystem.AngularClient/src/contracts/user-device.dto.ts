import { Dto } from "./dto";

export class UserDeviceDto extends Dto{
  userId: number;
  deviceId: number;
  userName: string;
  deviceName: string;
}
