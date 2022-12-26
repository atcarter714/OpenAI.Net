﻿namespace OpenAI.Net.Models.Requests
{
    public class ModerationRequestBase<T>
    {
      

        /// <summary>
        /// The input text to classify <br />
        /// <see href="https://beta.openai.com/docs/api-reference/moderations/create#moderations/create-input" />
        /// </summary>
        public T Input { get; set; }

        /// <summary>
        /// Two content moderations models are available: text-moderation-stable and text-moderation-latest. <br />
        /// The default is text-moderation-latest which will be automatically upgraded over time. <br />
        /// This ensures you are always using our most accurate model. <br />
        /// If you use text-moderation-stable, we will provide advanced notice before updating the model. <br />
        /// Accuracy of text-moderation-stable may be slightly lower than for text-moderation-latest. <br />
        /// <see href="https://beta.openai.com/docs/api-reference/moderations/create#moderations/create-model" />
        /// </summary>
        public string Model { get; set; }
    }
}
