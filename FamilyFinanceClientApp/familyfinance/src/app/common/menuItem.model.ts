// tslint:disable: variable-name

export class MenuItem {
  private _link = '';
  private _caption = '';
  private _icon = '';

  constructor(link: string, caption: string, icon: string) {
    this._link = link;
    this._caption = caption;
    this._icon = icon;
  }

  get link(): string {
    return this._link;
  }

  get caption(): string {
    return this._caption;
  }

  get icon(): string {
    return this._icon;
  }

}
