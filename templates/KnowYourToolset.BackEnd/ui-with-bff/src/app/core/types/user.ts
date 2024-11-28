export class User {
    public claims: Claim[] = [];
}

export class Claim {
    public type: string;
    public value: string;
  
    public constructor(type: string, value: string) {
      this.type = type;
      this.value = value;
    }
}