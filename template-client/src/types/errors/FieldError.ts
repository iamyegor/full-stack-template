export default class FieldError {
    problematicField: string;
    errorMessage: string;

    constructor(problematicField: string, errorMessage: string) {
        this.problematicField = problematicField;
        this.errorMessage = errorMessage;
    }

    static create(problematicField: string, errorMessage: string): FieldError {
        return new FieldError(problematicField, errorMessage);
    }

    isField(field: string): boolean {
        return this.problematicField === field;
    }

    forField(field: string): string | null {
        if (this.isField(field)) {
            return this.errorMessage;
        }
        return null;
    }
}
