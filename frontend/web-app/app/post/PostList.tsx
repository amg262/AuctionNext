'use client';

import {getPosts} from "@/app/actions/auctionActions";
import PostCard from "@/app/post/PostCard";
import {Post} from "@/types";
import {useEffect, useState} from "react";

export default function PostList() {
  const [loading, setLoading] = useState(true);
  const [posts, setPosts] = useState<Post[]>([]);
  const fetchData = async () => {
    const posts = await getPosts().then(res => res).catch(e => console.error(e));
    setPosts(posts);
    setLoading(true);
  }

  useEffect(() => {
    fetchData().then(() => setLoading(false)).catch(e => console.error(e));
  }, []);


  return (
      <div>
        <h2 className="text-2xl font-bold mb-6">Post List</h2>
        {loading && <h3>Loading...</h3>}
        <div className="grid grid-cols-1 md:grid-cols-3 lg:grid-cols-4 gap-6">
          {posts.map((post: Post) => (
              <PostCard post={post} key={post.id}/>
          ))}
        </div>
      </div>
  );
}