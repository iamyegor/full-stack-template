import type { Schema, Struct } from '@strapi/strapi';

export interface ElementsTodoList extends Struct.ComponentSchema {
  collectionName: 'components_elements_todo_lists';
  info: {
    description: '';
    displayName: 'Todo List';
  };
  attributes: {
    buttonText: Schema.Attribute.String;
    description: Schema.Attribute.Text;
    name: Schema.Attribute.String;
    slug: Schema.Attribute.String;
  };
}

declare module '@strapi/strapi' {
  export module Public {
    export interface ComponentSchemas {
      'elements.todo-list': ElementsTodoList;
    }
  }
}
