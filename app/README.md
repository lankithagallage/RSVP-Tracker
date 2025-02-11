# App

This project was generated using [Angular CLI](https://github.com/angular/angular-cli) version 19.1.2.

## Development server

To start a local development server, run:

```bash
ng serve
```

Once the server is running, open your browser and navigate to `http://localhost:4200/`. The application will automatically reload whenever you modify any of the source files.

## Code scaffolding

Angular CLI includes powerful code scaffolding tools. To generate a new component, run:

```bash
ng generate component component-name
```

For a complete list of available schematics (such as `components`, `directives`, or `pipes`), run:

```bash
ng generate --help
```

## Building

To build the project run:

```bash
ng build
```

This will compile your project and store the build artifacts in the `dist/` directory. By default, the production build optimizes your application for performance and speed.

## Running unit tests

To execute unit tests with the [Karma](https://karma-runner.github.io) test runner, use the following command:

```bash
ng test
```

## Running end-to-end tests

For end-to-end (e2e) testing, run:

```bash
ng e2e
```

Angular CLI does not come with an end-to-end testing framework by default. You can choose one that suits your needs.

## **Using `db.json` for Mock API Testing**

To simulate a REST API for testing, we use `json-server` with `db.json`.

#### **Install `json-server`**

```sh
npm install -g json-server
```

#### **Start the Mock API Server**

```sh
json-server --watch db.json --port 3000
```

This will host the API at `http://localhost:3000`.

#### **API Endpoints**

| Method  | Endpoint  | Description    |
| ------- | --------- | -------------- |
| **GET** | `/events` | Get all events |

#### **4️ Stop the Server**

Press `CTRL + C` to stop the server.
