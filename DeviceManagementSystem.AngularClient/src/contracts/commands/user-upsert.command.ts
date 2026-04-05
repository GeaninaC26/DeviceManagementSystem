export class UserUpsertCommand {
  name: string;
  email: string;
  location: string;
  password: string;

  constructor(init: Partial<UserUpsertCommand>) {
    Object.assign(this, init);
  }
}
