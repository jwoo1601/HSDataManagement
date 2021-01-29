import { injectable } from "inversify";
import "reflect-metadata";

export default interface IConfiguration {
  hasValue(key: string): boolean;
  getValue<TValue>(key: string): TValue | null;
}

@injectable()
export class DefaultConfigurationProvider implements IConfiguration {
  private loadedModule?: { [key: string]: any };

  constructor() {
    import(/* webpackChunkName: "config" */ `@/config/${process.env.NODE_ENV}`)
      .then((m) => {
        this.loadedModule = m;

        console.log(JSON.stringify(m));
      })
      .catch((err) => {
        throw Error(
          `Failed to load config module for env ${process.env.NODE_ENV}`
        );
      });
  }

  hasValue(key: string): boolean {
    return this.getValue(key) !== null;
  }

  getValue<TValue>(key: string): TValue | null {
    if (this.loadedModule) {
      return key
        .split(".")
        .reduce((obj, subPath) => obj[subPath], this.loadedModule) as TValue;
    }

    return null;
  }
}
