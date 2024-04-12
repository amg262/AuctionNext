'use client'


import {Post} from "@/types";
import {usePathname, useRouter} from "next/navigation";
import {FieldValues, useForm} from "react-hook-form";
import {useEffect} from "react";
import {createPost, updatePost} from "@/app/actions/auctionActions";
import {toast} from "react-hot-toast";
import Input from "@/app/components/Input";
import {Button} from "flowbite-react";
import {User} from "next-auth";

type Props = {
  post?: Post
  user?: User | null;
}

export default function PostForm({post, user}: Props) {

  console.log(user);
  const userId = user?.username;

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
      const {title, content, imageUrl, category, userId: id} = post;
      reset({title, content, imageUrl, category, userId, user: user?.username});
    }
    setFocus('title');
  }, [setFocus, reset, post, userId, user?.username])

  async function onSubmit(data: FieldValues) {
    try {
      // Ensure user data and post status are included
      const postData = {
        ...data,
        userId: user?.username, // Set user ID from current user
        status: 'Published' // Set status as Published
      };

      let id = '';
      let res;
      if (pathname.includes('/post/create')) {
        res = await createPost(postData); // Changed from createAuction
        id = res.id;
      } else {
        if (post) {
          res = await updatePost(postData, post.id); // Changed from updateAuction
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
        <Input label='Category' name='category' control={control}
               rules={{required: 'Category is required'}}/>
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
