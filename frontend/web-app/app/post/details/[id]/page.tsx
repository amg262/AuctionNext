import React from 'react'
import Image from 'next/image';
import {getPost} from "@/app/actions/auctionActions";
import {Post} from "@/types";

export default async function PostDetails({params}: { params: { id: string } }) {
  let error = null;
  let post: Post = {} as Post;

  try {
    post = await getPost(params.id);
    // post = postData;
    console.log('post - comp', post)
  } catch (err: any) {
    error = err;
  }


  console.log('payment', post)


  if (error) {
    return <div>Error: {error}</div>;
  }

  if (!post) {

    getPost(params.id)
        .then((res) => {
          post = res;
          console.log('post', post)

          return res
        }).catch((err) => {
      return err
    });

    // return <div>Loading...</div>; // Optionally show a loading or not found message
  }


  return (
      <div className='max-w-4xl mx-auto mt-10 px-4'>
        <h1 className='text-4xl font-bold text-gray-800 mb-3'>{post.title}</h1>
        {post.imageUrl && (
            <div className='mb-6'>
              <Image src={post.imageUrl} alt={post.title} width={800} height={450} className='rounded'/>
            </div>
        )}
        <p className='text-gray-600 text-lg'>{post.content}</p>
        {post.userId && (
            <p className='text-gray-800 text-lg font-semibold mt-4'>Author: {post.userId}</p>
        )}
      </div>
  )
}
