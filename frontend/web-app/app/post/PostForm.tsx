'use client'


import {Post} from "@/types";
import {usePathname, useRouter} from "next/navigation";
import {FieldValues, useForm} from "react-hook-form";
import {useEffect} from "react";
import {createPost, updatePost} from "@/app/actions/auctionActions";
import {toast} from "react-hot-toast";
import Input from "@/app/components/Input";
import {Button} from "flowbite-react";

type Props = {
  post?: Post
}

export default function PostForm({post}: Props) {
  const router = useRouter();
  const pathname = usePathname();
  const {
    control, handleSubmit, setFocus, reset,
    formState: {isSubmitting, isValid}
  } = useForm({
    mode: 'onTouched'
  });

  useEffect(() => {
    if (post) {
      const {title, content, imageUrl, userId, category} = post;
      reset({title, content, imageUrl, userId, category});
    }
    setFocus('title');
  }, [setFocus, reset, post])

  async function onSubmit(data: FieldValues) {
    try {
      let id = '';
      let res;
      if (pathname.includes('/post/create')) {
        res = await createPost(data); // Changed from createAuction
        id = res.id;
      } else {
        if (post) {
          res = await updatePost(data, post.id); // Changed from updateAuction
          id = post.id;
        }
      }
      if (res.error) {
        throw res.error;
      }
      router.push(`/post/details/${id}`) // Adjust path
    } catch (error: any) {
      toast.error(error.status + ' ' + error.message)
    }
  }

  return (
      <form className='flex flex-col mt-3' onSubmit={handleSubmit(onSubmit)}>
        <Input label='Title' name='title' control={control}
               rules={{required: 'Title is required'}}/>
        <Input label='Content' name='content' control={control}
               rules={{required: 'Content is required'}}/>
        <Input label='Image URL' name='imageUrl' control={control}
               rules={{required: 'Image URL is required'}}/>

        <div className='flex justify-between'>
          <Button outline color='gray'>Cancel</Button>
          <Button
              isProcessing={isSubmitting}
              disabled={!isValid}
              type='submit'
              outline color='success'>Submit</Button>
        </div>
      </form>
  )
}
