import Heading from '@/app/components/Heading'
import React from 'react'
import PostForm from "@/app/post/PostForm";

export default function Create() {
  return (
      <div className='mx-auto max-w-[75%] shadow-lg p-10 bg-white rounded-lg'>
        <Heading title='Create a post' subtitle='Add information needed in post'/>
        <PostForm/>
      </div>
  )
}
