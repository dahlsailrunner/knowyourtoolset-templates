# KnowYourToolset.BackEnd UI

## Angular Notes

Angular Material is used in this project.

## Backend For Frontend

### Authentication

The out-of-the-box code will allow anonymous access to the home page.

The top toolbar will show either a login button (if the user hasn't logged in), or the user
name with a logout button next to it if they have logged in.

The authentication is controlled by the BFF -- and it makes the `acccount/login`, `account/user`,
and `account/logout` endpoints available.

If you wanted to force authentication for EVERY page in the application -- even the home
page, you would comment IN the `initializeAuth` function in the `src/app/core/auth.service.ts` file,
and also the reference to it in the `src/app/app.config.ts` for the `APP_INITIALIZER`.

### Authorization

The demo IdentityServer has two accounts (bob and alice) that you can use to sign in.
There is an `authGuard` that simply requires an authenticated user - and that guard is
used on the `/user-claims` page.

There is also a `requiredClaimValueGuard` that takes a claim name and value
that are required to access the `/products` page.

This is configured in the `src/app/app.routes.ts` file, and the code for the
guards is in the `src/app/core/auth.guard.ts` file.

### Remote API Setup

The BFF looks for configuration of the form `RemoteApis:path` and will add a remote API
reference that will pass along the access token of the logged in user.

So in the already-coded example, a value of `RemoteApis:api` is configured to point
to the https endpoint of the API in the Aspire solution.  Then the UI code can
make calls to `api/<whatever>` that will get to the API project in this solution
with a bearer token.  See the `src/app/products/product.service.ts` for example of
how to code these calls.

Note that an HTTP interceptor has also been included to ensure that a CSRF token
gets included in every request.

## Hosting Setup

### Container Build Process
