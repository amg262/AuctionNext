"use server";

import axios from "axios";

export async function getPosts() {
  const {data} = await axios.get('https://jsonplaceholder.typicode.com/posts');
  return data;
}