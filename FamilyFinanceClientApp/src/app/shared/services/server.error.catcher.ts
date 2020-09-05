import { HttpErrorResponse } from '@angular/common/http';
import { ServerResponseError } from '../models/server.response.error.model';
import { Observable, throwError } from 'rxjs';

export function throwCustomError(error: HttpErrorResponse): Observable<ServerResponseError> {
    const serverResponseError = {} as ServerResponseError;
    serverResponseError.errorNumber = error.status;
    serverResponseError.message = error.message;

    switch (serverResponseError.errorNumber) {
        case 400:
            serverResponseError.description = 'Сервер вернул ошибку! 400 Данные запроса неверны!';
            break;
        case 401:
            serverResponseError.description = 'Сервер вернул ошибку! 401 Пользователь не авторизован!';
            break;
        case 403:
            serverResponseError.description =
                'Сервер вернул ошибку! 403 Данные заблокированы или у Вас нет прав выполнять данное действие!';
            break;
        case 404:
            serverResponseError.description = 'Сервер вернул ошибку! 404 Данные не найдены!';
            break;
        case 500:
            serverResponseError.description = 'Сервер вернул ошибку! 500 Внутренняя ошибка сервера!';
            break;
        default:
            serverResponseError.description = `Сервер вернул ошибку! ${serverResponseError.errorNumber} Нераспознанная ошибка!`;
            break;
    }
    return throwError(serverResponseError);
}
