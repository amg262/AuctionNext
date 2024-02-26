import {useParamsStore} from "@/hooks/useParamsStore";
import {Button} from "flowbite-react";
import Heading from "@/app/components/Heading";

type Props = {
  title?: string;
  subtitle?: string;
  showReset?: boolean;
}
export default function EmptyFilter({
                                      title = "No matches for this filter",
                                      subtitle = "Try changing or reseting filter",
                                      showReset
                                    }: Props) {

  const reset = useParamsStore(state => state.reset);

  return (
      <div className='h-[40vh] flex flex-col gap-2 justify-center items-center shadow-lg'>
        <Heading title={title} subtitle={subtitle} center/>
        <div className='mt-4'>
          {showReset && (
              <Button outline onClick={reset}>Remove Filters</Button>
          )}
        </div>
      </div>
  );
}