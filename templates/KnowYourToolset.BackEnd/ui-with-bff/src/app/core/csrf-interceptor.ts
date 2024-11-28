import { HttpInterceptorFn } from '@angular/common/http';

export const csrfInterceptor: HttpInterceptorFn = (req, next) => { 
  const reqWithCsrf = req.clone({
    setHeaders: { 'X-CSRF': '1', }
  });

  return next(reqWithCsrf);
};
