// // import { BootstrapContext, bootstrapApplication } from '@angular/platform-browser';
// // import { App } from './app/app';
// // import { config } from './app.config.server';

// // const bootstrap = (context: BootstrapContext) =>
// //     bootstrapApplication(App, config, context);

// // export default bootstrap;


// // src/main.server.ts
// import { BootstrapContext, bootstrapApplication } from '@angular/platform-browser';
// import { App } from './app/app';
// import { config } from './app.config.server';

// const bootstrap = (context: BootstrapContext) =>
//   bootstrapApplication(App, config, context);

// export default bootstrap;


import { BootstrapContext, bootstrapApplication } from '@angular/platform-browser';
import { App } from './app/app';
import { config } from './app.config.server';

const bootstrap = (context: BootstrapContext) =>
  bootstrapApplication(App, config, context);

export default bootstrap;
