<template>
  <div :class="componentClass">
    <h3 :class="headerClass">
      <b-icon :icon="headerIcon" class="mr-3"></b-icon>
      {{ $t(header) }}
      <hsm-button
        v-if="collapsible"
        class="ml-2"
        :icon="postsVisible ? 'chevron-up' : 'chevron-down'"
        :variant="variant"
        :text-variant="headerVariant"
        :hover-variant="headerVariant"
        :hover-text-variant="variant"
        text=""
        spacing="0"
        @click="postsVisible = !postsVisible"
      ></hsm-button>
    </h3>
    <hr :class="[`bg-${headerVariant}`, 'mt-2', 'mb-3']" style="height: 2px" />
    <b-collapse v-model="postsVisible">
      <ul id="posts" class="px-1">
        <li
          :key="post.id"
          v-for="post in tempPosts"
          class="d-flex justify-content-between px-3 py-2"
        >
          <b-container>
            <b-row class="d-flex align-items-center">
              <b-col cols="4" sm="5" xl="7" class="post-title h6">
                <!-- <b-icon icon="pencil" class="mr-3"></b-icon> -->
                <b-badge :variant="headerVariant" class="mr-3">
                  {{ post.id }}
                </b-badge>
                <b-link class="post-link">{{ post.title }}</b-link>
              </b-col>
              <b-col cols="8" sm="7" xl="5" class="post-stats">
                <div class="post-stats-item post-likes">
                  <b-icon icon="hand-thumbs-up" class="mr-2"></b-icon>
                  <span>{{ formatNumber(post.like_count) }}</span>
                </div>
                <div class="post-stats-item post-views">
                  <b-icon icon="eye" class="ml-3 mr-2"></b-icon>
                  <span>{{ formatNumber(post.view_count) }}</span>
                </div>
                <div class="post-stats-item post-comments">
                  <b-icon icon="card-text" class="ml-3 mr-2"></b-icon>
                  <span>{{ formatNumber(post.comments.length) }}</span>
                </div>
              </b-col>
            </b-row>
          </b-container>
        </li>
      </ul>
    </b-collapse>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";
import { Prop, Watch, Emit } from "vue-property-decorator";
import HSMPost from "@/models/api/Post";
import { BvTableField } from "bootstrap-vue";
import numeral from "numeral";

@Component({
  components: {},
})
export default class HSMPostList extends Vue {
  @Prop({ default: "title.posts" })
  readonly header!: string;
  @Prop({ default: "list-ul" })
  readonly headerIcon!: string;
  @Prop({ default: "light" })
  readonly variant!: string;
  @Prop({ default: "submain" })
  readonly headerVariant!: string;
  @Prop({ default: "dark" })
  readonly textVariant!: string;
  @Prop({ default: false })
  readonly collapsible!: boolean;

  postsVisible = true;
  tempPosts: HSMPost[] = [
    {
      id: 1,
      title: "New Post Test 111!",
      writer: "fsdfasdsadfas",
      created_at: new Date(),
      last_updated_at: new Date(),
      like_count: 3,
      view_count: 30,
      comments: [],
    },
    {
      id: 2,
      title: "Second Post @@!",
      writer: "fsdfasdsadfas",
      created_at: new Date(),
      last_updated_at: new Date(),
      like_count: 50000000000,
      view_count: 1243223,
      comments: [],
    },
    {
      id: 3,
      title: "Third Post 33333!",
      writer: "fsdfasdsadfas",
      created_at: new Date(),
      last_updated_at: new Date(),
      like_count: 54,
      view_count: 6823,
      comments: [],
    },
  ];

  get componentClass() {
    return ["px-4", "py-3", `bg-${this.variant}`];
  }

  get headerClass() {
    return [`text-${this.headerVariant}`, "px-3"];
  }

  formatNumber(num: number) {
    return numeral(num).format("0[.]0a");
  }
}
</script>

<style lang="scss" scoped>
#posts {
  list-style: none;

  li {
    border-top: 1px solid var(--gray);

    &:last-child {
      border-bottom: 1px solid var(--gray);
    }
  }
}

.post-title {
  font-weight: 500;
  margin: 0;

  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;

  .post-link {
    color: var(--dark);
  }
}

.post-stats {
  display: flex;
  justify-content: start;

  .post-stats-item {
    flex-grow: 1;
    flex-basis: 16px;
  }
}
</style>
