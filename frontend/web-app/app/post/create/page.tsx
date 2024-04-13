import Heading from '@/app/components/Heading'
import React from 'react'
import PostForm from "@/app/post/PostForm";
import {getCurrentUser} from "@/app/actions/authActions";
import Footer from "@/app/components/Footer";

export default async function Create() {
  const user = await getCurrentUser();
  return (
      <div className='mx-auto max-w-[75%] shadow-lg p-10 bg-white rounded-lg'>
        <Heading title='Create a post' subtitle='Add information needed in post'/>
        <PostForm user={user}/>
        <Footer/>
      </div>
  )
}
