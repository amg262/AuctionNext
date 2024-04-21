import React from 'react'
import Image from 'next/image';
import {createComment, getPost, getPostComments} from "@/app/actions/auctionActions";
import {Post, PostComment} from "@/types";
import Footer from "@/app/components/Footer";
import PostComments from "@/app/post/PostComments";
import PostCommentForm from "@/app/post/PostCommentForm";
import {getCurrentUser} from "@/app/actions/authActions";
import PostCommentsWithForm from "@/app/post/PostCommentsWithForm";

export default async function PostDetails({params}: { params: { id: string } }) {
  const user = await getCurrentUser();
  let error = null;
  let post: Post = {} as Post;
  let postComments: PostComment[] = [];

  try {
    post = await getPost(params.id);
    // post = postData;
    console.log('post - comp', post)
  } catch (err: any) {
    error = err;
  }

  try {
    postComments = await getPostComments(params.id);
    // post = postData;
    console.log('comment - comp', postComments)
  } catch (err: any) {
    error = err;
  }

  async function createPostComment(postId: string, commentContent: any) {
    try {
      // Assuming you have a function to call your API
      const response = await createComment(postId, commentContent);
      // You might want to update the local state to show the comment immediately
      console.log('Comment created', response);
    } catch (error) {
      console.error('Failed to post comment', error);
    }
  }

  // async function createComment() {
  //   console.log('Create comment')
  // }

  if (error) {
    return <div>Error: {error}</div>;
  }

  if (!post) {
    //
    // getPost(params.id)
    //     .then((res) => {
    //       post = res;
    //       console.log('post', post)
    //
    //       return res
    //     }).catch((err) => {
    //   return err
    // });

    return <div>Loading...</div>; // Optionally show a loading or not found message
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
        <PostCommentsWithForm key={post.id} post={post} username={user?.username}/>

        {post.userId !== user?.username && (
            // <PostCommentForm postId={post.id} username={user?.username}/>
            <>
            </>
        )}
        {/*<PostComments key={post.id} post={post} userId={post.userId}/>*/}
        <br/>
        <br/>
        <Footer/>
      </div>
  )
}
