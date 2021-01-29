<template>
  <div class="footer bg-dark text-light text-center pt-5 pb-3">
    <b-container>
      <b-row>
        <b-col cols="8" offset="2">
          <ul class="link-group group-align-h">
            <li>Customers</li>
            <li>
              Nutrition support
              <ul class="pl-3 group-align-v">
                <li>Service Management</li>
                <li>Food Management</li>
                <li>Menu Management</li>
              </ul>
            </li>
          </ul>
        </b-col>
      </b-row>
    </b-container>
    <hr id="content-divider" class="bg-submain" />
    <p class="font-weight-bold">Copyright © 2021 Jeongwoo Kim</p>
  </div>
</template>

<script lang="ts">
import Vue from "vue";
import Component from "vue-class-component";

@Component({
  components: {},
})
export default class Footer extends Vue {}
</script>

<style lang="scss" scoped>
#content-divider {
  width: 80%;
  height: 2px;
  margin: 16px auto;
}

.link-group {
  display: flex;
  list-style: none;
  text-align: left;

  li {
    &:not(:last-child) {
      margin-right: 60px;
    }
  }
}

.group-align-h li {
  display: inline-block;
}

.group-align-v li {
  display: block;
}
</style>
>
