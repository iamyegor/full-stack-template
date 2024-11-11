"use strict"

/**
 * `test` middleware.
 */

const populate = ["todoLists"]

module.exports = (config, { strapi }) => {
  // Add your own logic here.
  return async (ctx, next) => {
    ctx.query.locale = ctx.request.url.split("locale=")[1]?.slice(0, 2)
    ctx.query.populate = populate

    await next()
  }
}
