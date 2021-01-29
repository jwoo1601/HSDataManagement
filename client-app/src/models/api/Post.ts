import { Transform, Type } from "class-transformer";

export interface RawHSMPostCategory {
  id: number;
  name: string;
  created_at: string;
  last_updated_at: string;
}

export class HSMPostCategory {
  id!: number;
  name!: string;
  @Type(() => Date)
  created_at!: Date;
  @Type(() => Date)
  last_updated_at!: Date;
}

export interface RawHSMCommentReply {
  id: number;
  text: string;
  comment: number;
  writer: string;
  like_count: number;
  created_at: string;
  last_updated_at: string;
}

export class HSMCommentReply {
  id!: number;
  text!: string;
  comment!: number;
  writer!: string;
  like_count!: number;
  @Type(() => Date)
  created_at!: Date;
  @Type(() => Date)
  last_updated_at!: Date;
}

export interface RawHSMPostComment {
  id: number;
  text: string;
  post: number;
  writer: string;
  replies: RawHSMCommentReply[];
  like_count: number;
  created_at: string;
  last_updated_at: string;
}

export class HSMPostComment {
  id!: number;
  text!: string;
  post!: number;
  writer!: string;
  @Type(() => HSMCommentReply)
  replies!: HSMCommentReply[];
  like_count!: number;
  @Type(() => Date)
  created_at!: Date;
  @Type(() => Date)
  last_updated_at!: Date;
}

export interface RawHSMPost {
  id: number;
  title: string;
  content?: string;
  category?: RawHSMPostCategory;
  writer: string;
  view_count: number;
  like_count: number;
  comments: RawHSMPostComment[];
  created_at: string;
  last_updated_at: string;
}

export default class HSMPost {
  id!: number;
  title!: string;
  content?: string;
  @Type(() => HSMPostCategory)
  category?: HSMPostCategory;
  writer!: string;
  view_count!: number;
  like_count!: number;
  @Type(() => HSMPostComment)
  comments!: HSMPostComment[];
  @Type(() => Date)
  created_at!: Date;
  @Type(() => Date)
  last_updated_at!: Date;
}

export interface HSMPostInputModel {
  name: string;
  note?: string;
  duration?: string;
  group?: number;
}

export interface HSMPostCategoryInputModel {
  name: string;
  note?: string;
}
