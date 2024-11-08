export default class RouteError extends Error {
    title: string;

    private constructor(title: string, message: string) {
        super(message);
        this.title = title;
    }

    static notFound(): RouteError {
        return new RouteError("404 - Страница не найдена", "Упс! Чат или страница, которую вы ищете, не существует.");
    }

    static unexpected(): RouteError {
        return new RouteError(
            "Какая-то Ошибка",
            "Что-то пошло не так. Пожалуйста, попробуйте еще раз.",
        );
    }

    static serverError(): RouteError {
        return new RouteError(
            "500! Ошибка сервера",
            "Ошибка на нашей стороне. Пожалуйста, попробуйте позже.",
        );
    }
}
